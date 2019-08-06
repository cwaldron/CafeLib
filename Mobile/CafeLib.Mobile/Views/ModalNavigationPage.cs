﻿using Xamarin.Forms;

namespace CafeLib.Mobile.Views
{
    public class ModalNavigationPage : NavigationPage, INavigableOwner
    {
        /// <summary>
        /// Page owner
        /// </summary>
        public INavigableOwner Owner { get; internal set; }

        /// <summary>
        /// CafeNavigationPage constructor.
        /// </summary>
        /// <param name="root">root page</param>
        public ModalNavigationPage(Page root)
            : base(root)
        {
            SetOwner(root, this);
            BarBackgroundColor = root.BackgroundColor;

            Pushed += (s, e) =>
            {
                SetOwner(e.Page, this);
            };

            Popped += (s, e) =>
            {
                SetOwner(e.Page, null);
            };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="page"></param>
        /// <param name="owner"></param>
        private static void SetOwner(Page page, INavigableOwner owner)
        {
            switch (page)
            {
                case BaseContentPage contentPage:
                    contentPage.Owner = owner;
                    break;

                case BaseMasterDetailPage masterDetailPage:
                    masterDetailPage.Owner = owner;
                    break;

                case ModalNavigationPage navigationPage:
                    navigationPage.Owner = owner;
                    break;
            }
        }
    }
}