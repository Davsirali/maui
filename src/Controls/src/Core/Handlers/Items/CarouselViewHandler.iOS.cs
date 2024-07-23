﻿#nullable disable
using System;
using Microsoft.Maui.Graphics;
using UIKit;

namespace Microsoft.Maui.Controls.Handlers.Items
{
	public partial class CarouselViewHandler : ItemsViewHandler<CarouselView>
	{
		//UICollectionViewLayout _layout;

		protected override CarouselViewController CreateController(CarouselView newElement, UICollectionViewLayout layout)
				=> new CarouselViewController(newElement, layout);

		protected override UICollectionViewLayout SelectLayout() =>
				 LayoutFactory.CreateCarousel(VirtualView.ItemsLayout, new LayoutGroupingInfo(), () => VirtualView.PeekAreaInsets);

		protected override void ScrollToRequested(object sender, ScrollToRequestEventArgs args)
		{
			if (VirtualView?.Loop == true)
			{
				var goToIndexPath = (Controller as CarouselViewController).GetScrollToIndexPath(args.Index);

				if (!IsIndexPathValid(goToIndexPath))
				{
					return;
				}

				Controller.CollectionView.ScrollToItem(goToIndexPath,
					args.ScrollToPosition.ToCollectionViewScrollPosition( UICollectionViewScrollDirection.Vertical), // TODO: Fix _layout.ScrollDirection),
					args.IsAnimated);
			}
			else
			{
				base.ScrollToRequested(sender, args);
			}
		}

		public static void MapIsSwipeEnabled(CarouselViewHandler handler, CarouselView carouselView)
		{
			handler.Controller.CollectionView.ScrollEnabled = carouselView.IsSwipeEnabled;
		}

		public static void MapIsBounceEnabled(CarouselViewHandler handler, CarouselView carouselView)
		{
			handler.Controller.CollectionView.Bounces = carouselView.IsBounceEnabled;
		}

		public static void MapPeekAreaInsets(CarouselViewHandler handler, CarouselView carouselView)
		{
			// TODO: Fix (handler.Controller.Layout as CarouselViewLayout)?.UpdateConstraints(handler.PlatformView.Frame.Size);
			handler.UpdateLayout();
		//	handler.Controller.CollectionView.ReloadData();
		}

		public static void MapCurrentItem(CarouselViewHandler handler, CarouselView carouselView)
		{
			(handler.Controller as CarouselViewController)?.UpdateFromCurrentItem();
		}

		public static void MapPosition(CarouselViewHandler handler, CarouselView carouselView)
		{
			// If the initial position hasn't been set, we have a UpdateInitialPosition call on CarouselViewController
			// that will handle this so we want to skip this mapper call. We need to wait for the CollectionView to be ready
			if(handler.Controller is CarouselViewController carouselViewController && carouselViewController.InitialPositionSet)
			{
				carouselViewController.UpdateFromPosition();
			}
		}

		public static void MapLoop(CarouselViewHandler handler, CarouselView carouselView)
		{
			(handler.Controller as CarouselViewController)?.UpdateLoop();
		}

		// public override Size GetDesiredSize(double widthConstraint, double heightConstraint) => 
		// 	this.GetDesiredSizeFromHandler(widthConstraint, heightConstraint);
	}
}