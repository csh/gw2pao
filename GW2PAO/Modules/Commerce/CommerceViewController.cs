﻿using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GW2PAO.Modules.Commerce.Interfaces;
using GW2PAO.Modules.Commerce.Views;
using GW2PAO.Modules.Commerce.Views.PriceNotification;
using GW2PAO.Modules.Commerce.Views.PriceTracker;
using GW2PAO.Utility;
using NLog;

namespace GW2PAO.Modules.Commerce
{
    [Export(typeof(ICommerceViewController))]
    public class CommerceViewController : ICommerceViewController
    {
        /// <summary>
        /// Default logger
        /// </summary>
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Composition container of composed parts
        /// </summary>
        [Import]
        private CompositionContainer Container { get; set; }

        /// <summary>
        /// The TP Calculator utility window
        /// </summary>
        private TPCalculatorView tpCalculatorView;

        /// <summary>
        /// Window containing price notifications
        /// </summary>
        private PriceNotificationWindow priceNotificationsView;

        /// <summary>
        /// Window used for configuring item price watches
        /// </summary>
        private PriceWatchConfigView priceWatchConfigView;

        /// <summary>
        /// View used when rebuilding the item names database
        /// </summary>
        private RebuildNamesDatabaseView rebuildItemNamesView;

        /// <summary>
        /// The price tracker view
        /// </summary>
        private PriceTrackerView priceTrackerView;

        /// <summary>
        /// Displays all previously-opened windows and other windows
        /// that must be shown at startup
        /// </summary>
        public void Initialize()
        {
            logger.Debug("Initializing");
            Threading.BeginInvokeOnUI(() =>
            {
                if (Properties.Settings.Default.IsTPCalculatorOpen && this.CanDisplayTPCalculator())
                    this.DisplayTPCalculator();

                if (Properties.Settings.Default.IsPriceTrackerOpen && this.CanDisplayPriceTracker())
                    this.DisplayPriceTracker();

                if (this.CanDisplayPriceNotificationsWindow())
                    this.DisplayPriceNotificationsWindow();
            });
        }

        /// <summary>
        /// Closes all windows and saves the "was previously opened" state for those windows.
        /// </summary>
        public void Shutdown()
        {
            logger.Debug("Shutting down");
            
            if (this.tpCalculatorView != null)
            {
                Properties.Settings.Default.IsTPCalculatorOpen = this.tpCalculatorView.IsVisible;
                Threading.InvokeOnUI(() => this.tpCalculatorView.Close());
            }

            if (this.priceTrackerView != null)
            {
                Properties.Settings.Default.IsPriceTrackerOpen = this.priceTrackerView.IsVisible;
                Threading.InvokeOnUI(() => this.priceTrackerView.Close());
            }

            Properties.Settings.Default.Save();
        }

        /// <summary>
        /// Displays the TP Calculator window, or, if already displayed,
        /// sets focus to the window
        /// </summary>
        public void DisplayTPCalculator()
        {
            if (this.tpCalculatorView == null || !this.tpCalculatorView.IsVisible)
            {
                this.tpCalculatorView = new TPCalculatorView();
                this.Container.ComposeParts(this.tpCalculatorView);
                this.tpCalculatorView.Show();
            }
            else
            {
                this.tpCalculatorView.Focus();
            }
        }

        /// <summary>
        /// Determines if the TP Calculator can be displayed
        /// </summary>
        /// <returns></returns>
        public bool CanDisplayTPCalculator()
        {
            return true;
        }

        /// <summary>
        /// Displays the Price Notifications Configuration window, or, if already displayed,
        /// sets focus to the window
        /// </summary>
        public void DisplayPriceNotificationsConfig()
        {
            if (this.priceWatchConfigView == null || !this.priceWatchConfigView.IsVisible)
            {
                this.priceWatchConfigView = new PriceWatchConfigView();
                this.Container.ComposeParts(this.priceWatchConfigView);
                this.priceWatchConfigView.Show();
            }
            else
            {
                this.priceWatchConfigView.Focus();
            }
        }

        /// <summary>
        /// Determines if the Price Notifications Configuration window can be displayed
        /// </summary>
        public bool CanDisplayPriceNotificationsConfig()
        {
            return true;
        }

        /// <summary>
        /// Displays the Price Rebuild Item Names Database window, or, if already displayed,
        /// sets focus to the window
        /// </summary>
        public void DisplayRebuildItemNamesView()
        {
            if (this.rebuildItemNamesView == null || !this.rebuildItemNamesView.IsVisible)
            {
                this.rebuildItemNamesView = new RebuildNamesDatabaseView();
                this.Container.ComposeParts(this.rebuildItemNamesView);
                this.rebuildItemNamesView.Show();
            }
            else
            {
                this.priceWatchConfigView.Focus();
            }
        }

        /// <summary>
        /// Determines if the Rebuild Item Names Database window can be displayed
        /// </summary>
        public bool CanDisplayRebuildItemNamesView()
        {
            return true;
        }

        /// <summary>
        /// Displays the Price Tracker window, or, if already displayed, sets
        /// focus to the window
        /// </summary>
        public void DisplayPriceTracker()
        {
            if (this.priceTrackerView == null || !this.priceTrackerView.IsVisible)
            {
                this.priceTrackerView = new PriceTrackerView();
                this.Container.ComposeParts(this.priceTrackerView);
                this.priceTrackerView.Show();
            }
            else
            {
                this.priceTrackerView.Focus();
            }
        }

        /// <summary>
        /// Determines if the price tracker can be displayed
        /// </summary>
        /// <returns>Always true</returns>
        public bool CanDisplayPriceTracker()
        {
            return true;
        }

        /// <summary>
        /// Displays the Price Notifications window
        /// </summary>
        public void DisplayPriceNotificationsWindow()
        {
            if (this.priceNotificationsView == null || !this.priceNotificationsView.IsVisible)
            {
                this.priceNotificationsView = new PriceNotificationWindow();
                this.Container.ComposeParts(this.priceNotificationsView);
                this.priceNotificationsView.Show();
            }
            else
            {
                this.priceNotificationsView.Focus();
            }
        }

        /// <summary>
        /// Determines if the Price Notifications window can be displayed
        /// </summary>
        /// <returns>Always true</returns>
        public bool CanDisplayPriceNotificationsWindow()
        {
            return true;
        }
    }
}