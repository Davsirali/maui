﻿using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Maui.Graphics;
using System.Linq;
using Microsoft.Maui.ApplicationModel;

namespace Microsoft.Maui.Controls
{
	// Once MAUI is capable of packaging styles into XAML files we will have implicit styles
	// that we can add as the base for the DefaultDictionary
	internal static class DefaultStyles
	{
		static Dictionary<Type, Style> DefaultStylesCache { get; } = new Dictionary<Type, Style>();


		static T GetThemeChoice<T>(T light, T dark, AppTheme? appTheme = null)
		{
			if ((appTheme ?? Application.Current?.RequestedTheme) == AppTheme.Dark)
				return dark;

			return light;
		}

		class LightTheme
		{
			public static Color TextColor => Colors.Black;
			public static Color PickerTitleColor => Colors.White;
			public static Color BackgroundColor = Colors.White;

			// These all match the accent color set in android
			// in forms it was fuschia because that was the default in 
			// android studio 6 years ago when we switched to appcompat
			//
			// Here I've just picked the current blackish color we are using in MAUI
			public static Color AccentColor = new Color(52, 152, 219);
			public static Color CheckBoxColor => AccentColor;
			public static Color ProgressBarProgressColor => AccentColor;

			public static Color PlaceholderElementPlaceholderColor = TextColor.WithAlpha(0.50f);
			public static Color SearchBarCancelButtonColor => ButtonBackgroundColor;

#if ANDROID
			public static Color ButtonBackgroundColor = new Color(44, 62, 80);
			public static Color ButtonTextColor => Colors.White;
			public static Color SwitchOnColor => null;
			public static Color ActivityIndicatorColor => AccentColor;
			public static Color SliderMaximumTrackColor = SliderThumbColor.WithAlpha(0.50f);
			public static Color SliderMinimumTrackColor => Colors.Black;
			public static Color SwitchThumbColor => AccentColor;
			public static Color SliderThumbColor => AccentColor;
#else
			public static Color SwitchThumbColor => null;
			public static Color SliderThumbColor => null;
			public static Color ButtonBackgroundColor => Colors.White;
			public static Color ButtonTextColor => new(0, 122, 255);
			public static Color SwitchOnColor => null;
			public static Color ActivityIndicatorColor = new Color(0, 0, 0, 114);
			public static Color SliderMinimumTrackColor => null;
			public static Color SliderMaximumTrackColor => null;
#endif
		}

		class DarkTheme
		{
			public static Color BackgroundColor = Colors.Black;

			// These all match the accent color set in android
			// in forms it was fuschia because that was the default in 
			// android studio 6 years ago when we switched to appcompat
			//
			// Here I've just picked the current blackish color we are using in MAUI
			public static Color AccentColor = new Color(52, 152, 219);
			public static Color CheckBoxColor => AccentColor;
			public static Color ProgressBarProgressColor => AccentColor;

			public static Color PlaceholderElementPlaceholderColor = TextColor.WithAlpha(0.38f);
			public static Color PickerTitleColor => TextColor;
			public static Color TextColor => Colors.White;

			public static Color SearchBarCancelButtonColor => ButtonBackgroundColor;
#if ANDROID
			public static Color ButtonBackgroundColor = new Color(44, 62, 80);
			public static Color ButtonTextColor => Colors.Black;
			public static Color SwitchOnColor => null;
			public static Color SliderMinimumTrackColor => SliderThumbColor;
			public static Color SliderMaximumTrackColor = SliderThumbColor.WithAlpha(0.50f);
			public static Color ActivityIndicatorColor => AccentColor;
			public static Color SwitchThumbColor => AccentColor;
			public static Color SliderThumbColor => AccentColor;
#else
			public static Color ButtonBackgroundColor => Colors.Black;
			public static Color ButtonTextColor => new(0, 122, 255);
			public static Color SwitchThumbColor => null;
			public static Color SliderThumbColor => null;
			public static Color SwitchOnColor => null;
			public static Color ActivityIndicatorColor = new Color(0, 0, 0, 114);
			public static Color SliderMinimumTrackColor => null;
			public static Color SliderMaximumTrackColor => null;
#endif
		}


		static Setter GetSetterFor(Type viewType, BindableProperty bindableProperty, out Style style)
		{
			if (DefaultStylesCache.TryGetValue(viewType, out Style existing))
			{
				style = existing;
				foreach (var setter in style.Setters)
				{
					if (setter.Property == bindableProperty)
						return setter;
				}
			}
			else
			{
				style = new Style(viewType);
				DefaultStylesCache[viewType] = style;
			}

			return null;
		}

		public static AppThemeBinding GetAppThemeBinding(
			Type viewType,
			BindableProperty bindableProperty,
			float withAlpha = 1f)
		{
			var dark = (GetColor(viewType, bindableProperty, AppTheme.Dark)?.Value as Color)?.WithAlpha(withAlpha);
			var light = (GetColor(viewType, bindableProperty, AppTheme.Light)?.Value as Color)?.WithAlpha(withAlpha);

			if (dark != null && light != null)
			{
				return new AppThemeBinding()
				{
					Dark = dark,
					Light = light
				};
			}

			return null;
		}

		public static Setter GetColor(BindableObject view, BindableProperty bindableProperty, AppTheme? appTheme = null) =>
			GetColor(view.GetType(), bindableProperty);

		public static Setter GetColor(Type viewType, BindableProperty bindableProperty, AppTheme? appTheme = null)
		{
			Setter setterToUse = GetSetterFor(viewType, bindableProperty, out Style _);
			if (setterToUse != null)
				return setterToUse;

#if ANDROID || IOS

			if (bindableProperty == TextElement.TextColorProperty)
			{
				if (viewType.IsAssignableTo(typeof(ITextElement)))
				{
					var textColorSetting = new Setter();
					textColorSetting.Property = TextElement.TextColorProperty;

					if (viewType.IsAssignableFrom(typeof(Button)))
						textColorSetting.Value = GetThemeChoice(LightTheme.ButtonTextColor, DarkTheme.ButtonTextColor, appTheme);
					else
						textColorSetting.Value = GetThemeChoice(LightTheme.TextColor, DarkTheme.TextColor, appTheme);

					setterToUse = textColorSetting;
				}
			}
			else if (bindableProperty == VisualElement.BackgroundColorProperty)
			{
				var backgroundColorSetter = new Setter();
				backgroundColorSetter.Property = VisualElement.BackgroundColorProperty;
				if (viewType.IsAssignableFrom(typeof(Button)))
				{
					backgroundColorSetter.Value = GetThemeChoice(LightTheme.ButtonBackgroundColor, DarkTheme.ButtonBackgroundColor, appTheme);
				}
				else
				{
					backgroundColorSetter.Value = GetThemeChoice(LightTheme.BackgroundColor, DarkTheme.BackgroundColor, appTheme);

				}

				setterToUse = backgroundColorSetter;
			}
			else if (bindableProperty == ActivityIndicator.ColorProperty)
			{
				setterToUse = new Setter();
				setterToUse.Value = GetThemeChoice(LightTheme.ActivityIndicatorColor, DarkTheme.ActivityIndicatorColor, appTheme);
			}
			else if (bindableProperty == CheckBox.ColorProperty)
			{
				setterToUse = new Setter();
				setterToUse.Value = GetThemeChoice(LightTheme.CheckBoxColor, DarkTheme.CheckBoxColor, appTheme);
			}
			else if (bindableProperty == PlaceholderElement.PlaceholderColorProperty)
			{
				setterToUse = new Setter();
				setterToUse.Value = GetThemeChoice(LightTheme.PlaceholderElementPlaceholderColor, DarkTheme.PlaceholderElementPlaceholderColor, appTheme);
			}
			else if (bindableProperty == Picker.TitleColorProperty)
			{
				setterToUse = new Setter();
				setterToUse.Value = GetThemeChoice(LightTheme.PickerTitleColor, DarkTheme.PickerTitleColor, appTheme);
			}
			else if (bindableProperty == ProgressBar.ProgressColorProperty)
			{
				setterToUse = new Setter();
				setterToUse.Value = GetThemeChoice(LightTheme.ProgressBarProgressColor, DarkTheme.ProgressBarProgressColor, appTheme);
			}
			else if (bindableProperty == SearchBar.CancelButtonColorProperty)
			{
				setterToUse = new Setter();
				setterToUse.Value = GetThemeChoice(LightTheme.SearchBarCancelButtonColor, DarkTheme.SearchBarCancelButtonColor, appTheme);
			}
			else if (bindableProperty == Slider.ThumbColorProperty)
			{
				setterToUse = new Setter();
				setterToUse.Value = GetThemeChoice(LightTheme.SliderThumbColor, DarkTheme.SliderThumbColor, appTheme);
			}
			else if (bindableProperty == Slider.MinimumTrackColorProperty)
			{
				setterToUse = new Setter();
				setterToUse.Value = GetThemeChoice(LightTheme.SliderMinimumTrackColor, DarkTheme.SliderMinimumTrackColor, appTheme);
			}
			else if (bindableProperty == Slider.MaximumTrackColorProperty)
			{
				setterToUse = new Setter();
				setterToUse.Value = GetThemeChoice(LightTheme.SliderMaximumTrackColor, DarkTheme.SliderMaximumTrackColor, appTheme);
			}
			else if (bindableProperty == Switch.ThumbColorProperty)
			{
				setterToUse = new Setter();

#if IOS
				setterToUse.Value = GetThemeChoice(LightTheme.SwitchThumbColor, DarkTheme.SwitchThumbColor, appTheme);
#endif

			}
			else if (bindableProperty == Switch.OnColorProperty)
			{
				setterToUse = new Setter();
				setterToUse.Value = GetThemeChoice(LightTheme.SwitchOnColor, DarkTheme.SwitchOnColor, appTheme);
			}

#endif

			if (setterToUse?.Value == null)
				return null;

			return setterToUse;
		}

		internal static VisualStateGroupList GetVisualStateManager(BindableObject bindable)
		{
			return GetVisualStateManager(bindable.GetType(), bindable);
		}

		internal static VisualStateGroupList GetVisualStateManager(Type viewType, BindableObject bindable = null)
		{
#if IOS || ANDROID

			VisualStateGroupList visualStateGroupList = null;
			var visualStateGroup = new VisualStateGroup() { Name = "CommonStates" };


			// This means we are retrieving this for a style not a specific bindable
			if (bindable == null)
			{
				var existing = GetSetterFor(viewType, VisualStateManager.VisualStateGroupsProperty, out Style style);
				if (existing != null)
				{
					return (VisualStateGroupList)existing.Value;
				}
			}

			List<VisualState> statesToAdd = new List<VisualState>();
			var disabledSetters = new VisualState()
			{
				Name = "Disabled"
			};


#if ANDROID
			var disabledBackgroundThemeBinding = GetAppThemeBinding(viewType, VisualElement.BackgroundColorProperty, 0.12f);

			if (disabledBackgroundThemeBinding != null && viewType.IsAssignableTo(typeof(Button)))
			{
				disabledSetters.Setters.Add(new Setter()
				{
					Property = VisualElement.BackgroundColorProperty,
					Value = disabledBackgroundThemeBinding
				});
			}


			if (viewType.IsAssignableTo(typeof(ITextElement)))
			{
				var disabledTextColor = new Setter()
				{
					Property = TextElement.TextColorProperty,
					Value = new Color(0f, 0f, 0f).WithAlpha(0.38f)
				};

				disabledSetters.Setters.Add(disabledTextColor);
			}
#endif

#if IOS
			if (viewType.IsAssignableTo(typeof(Button)))
			{
				var disabledTextColor = new Setter()
				{
					Property = TextElement.TextColorProperty,
					Value = new AppThemeBinding() { Dark = new Color(130, 130, 130).WithAlpha(89), Light = new Color(89, 130, 130).WithAlpha(89) }
				};

				disabledSetters.Setters.Add(disabledTextColor);
			}
#endif

			if (disabledSetters.Setters.Count > 0)
			{

				visualStateGroup.States.Add(new VisualState()
				{
					Name = "Normal"
				});

				visualStateGroup.States.Add(disabledSetters);
				visualStateGroupList ??= CreateVisualStateGroupList();
			}
#if ANDROID
			if (viewType.IsAssignableTo(typeof(Switch)))
			{
				visualStateGroupList ??= CreateVisualStateGroupList();

				visualStateGroup.States.Add(new VisualState()
				{
					Name = "On",
					Setters =
					{
						new Setter()
						{
							Property = Switch.ThumbColorProperty,
							Value = new AppThemeBinding()
							{
								Light = LightTheme.SwitchThumbColor,
								Dark = DarkTheme.SwitchThumbColor
							}
						}
					}
				});

				visualStateGroup.States.Add(new VisualState()
				{
					Name = "Off"
				});
			}
#endif

			return visualStateGroupList;

			VisualStateGroupList CreateVisualStateGroupList()
			{
				if (bindable == null)
				{
					visualStateGroupList = new VisualStateGroupList() { visualStateGroup };
					_ = GetSetterFor(viewType, VisualStateManager.VisualStateGroupsProperty, out Style style);
					style.Setters.Add(new Setter() { Property = VisualStateManager.VisualStateGroupsProperty, Value = visualStateGroupList });
				}
				else
				{
					visualStateGroupList = new VisualStateGroupList(true) { VisualElement = (VisualElement)bindable };
					visualStateGroupList.Add(visualStateGroup);
				}

				return visualStateGroupList;
			}
#else
			return null;
#endif
		}


		class DefaultResourceDictionary : ResourceDictionary { }

		public static ResourceDictionary CreateDefaultResourceDictionary()
		{
			Type[] controls = new[]
			{
				typeof(ActivityIndicator),
				typeof(Button),
				typeof(CheckBox),
				typeof(DatePicker),
				typeof(Editor),
				typeof(Entry),
				typeof(ImageButton),
				typeof(Label),
				typeof(Picker),
				typeof(ProgressBar),
				typeof(RadioButton),
				typeof(SearchBar),
				typeof(Slider),
				typeof(Stepper),
				typeof(Switch),
				typeof(TimePicker),
			};

			var returnValue = new DefaultResourceDictionary();

			foreach (var control in controls)
			{
				var style = CreateStyle(control);
				if (style != null)
					returnValue.Add(style);
			}

			return returnValue;
		}

		public static Style CreateStyle(Type controlType)
		{
			var text = GetColor(controlType, TextElement.TextColorProperty);
			var background = GetColor(controlType, VisualElement.BackgroundColorProperty);
			var vsm = GetVisualStateManager(controlType);

			var style = new Style(controlType);

			if (text != null)
				style.Setters.Add(text);

			if (background != null)
				style.Setters.Add(background);

			if (vsm != null)
				style.Setters.Add(new Setter() { Property = VisualStateManager.VisualStateGroupsProperty, Value = vsm });

			if (style.Setters.Count > 0)
				return style;

			return null;
		}
	}
}
