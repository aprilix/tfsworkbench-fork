﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ChildColourConverter.cs" company="None">
//   None
// </copyright>
// <summary>
//   Defines the ChildColourConverter type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using TfsWorkbench.UIElements.ValueConverters;

namespace TfsWorkbench.TaskBoardUI.Converters
{
    using System;
    using System.Globalization;
    using System.Linq;
    using System.Windows.Data;
    using System.Windows.Media;

    using TfsWorkbench.Core.Helpers;
    using TfsWorkbench.Core.Interfaces;
    using TfsWorkbench.TaskBoardUI.DataObjects;

    /// <summary>
    /// Initializes instance of ChildColourConverter
    /// </summary>
    public class ChildColourConverter : IMultiValueConverter
    {
        public BackgroundBrushConverter BackgoundConverter { get; set; }

        /// <summary>
        /// Converts source values to a value for the binding target. The data binding engine calls this method when it propagates the values from source bindings to the binding target.
        /// </summary>
        /// <param name="values">The array of values that the source bindings in the <see cref="T:System.Windows.Data.MultiBinding"/> produces. The value <see cref="F:System.Windows.DependencyProperty.UnsetValue"/> indicates that the source binding has no value to provide for conversion.</param>
        /// <param name="targetType">The type of the binding target property.</param>
        /// <param name="parameter">The converter parameter to use.</param>
        /// <param name="culture">The culture to use in the converter.</param>
        /// <returns>
        /// A converted value.If the method returns null, the valid null value is used.A return value of <see cref="T:System.Windows.DependencyProperty"/>.<see cref="F:System.Windows.DependencyProperty.UnsetValue"/> indicates that the converter did not produce a value, and that the binding will use the <see cref="P:System.Windows.Data.BindingBase.FallbackValue"/> if it is available, or else will use the default value.A return value of <see cref="T:System.Windows.Data.Binding"/>.<see cref="F:System.Windows.Data.Binding.DoNothing"/> indicates that the binding does not transfer the value or use the <see cref="P:System.Windows.Data.BindingBase.FallbackValue"/> or the default value.
        /// </returns>
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var defaultBrush = new SolidColorBrush(Core.Properties.Settings.Default.ItemColour);

            if (values == null || values.Length != 2)
            {
                return defaultBrush;
            }

            var workbenchItem = values[0] as IWorkbenchItem;
            var swimLaneView = values[1] as SwimLaneView;

            if (workbenchItem == null || swimLaneView == null)
            {
                return defaultBrush;
            }

            var currentState = workbenchItem.GetState();
            var title = workbenchItem.GetCaption();
            if (title.ToUpperInvariant().Contains("[NP]"))
            {
                return new SolidColorBrush(Color.FromRgb(255, 0, 0));
            }

            var stateColour = swimLaneView.ViewMap.StateItemColours.FirstOrDefault(sc => sc.Value.Equals(currentState));

            return stateColour == null || BackgoundConverter == null
                       ? defaultBrush
                       : BackgoundConverter.Convert(stateColour.ColourAsString, targetType, parameter, culture);
        }

        /// <summary>
        /// Converts a binding target value to the source binding values.
        /// </summary>
        /// <param name="value">The value that the binding target produces.</param>
        /// <param name="targetTypes">The array of types to convert to. The array length indicates the number and types of values that are suggested for the method to return.</param>
        /// <param name="parameter">The converter parameter to use.</param>
        /// <param name="culture">The culture to use in the converter.</param>
        /// <returns>
        /// An array of values that have been converted from the target value back to the source values.
        /// </returns>
        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
