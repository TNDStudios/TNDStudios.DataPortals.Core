﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TNDStudios.DataPortals.Helpers;

namespace TNDStudios.DataPortals.PropertyBag
{
    public class PropertyBagHelper
    {
        /// <summary>
        /// Property Bag Items referenced by the calling object
        /// </summary>
        private CommonObject commonObject { get; set; }

        /// <summary>
        /// Constructor that takes the list of items from the calling object
        /// </summary>
        /// <param name="propertyBag"></param>
        public PropertyBagHelper(CommonObject commonObject)
            => this.commonObject = commonObject;

        /// <summary>
        /// Get a configuration item from the property bag
        /// and format it as appropriate
        /// </summary>
        /// <typeparam name="T">The type of data that is requested</typeparam>
        /// <param name="key">The key for the data</param>
        /// <returns>The data formatted as the correct type</returns>
        public T Get<T>(PropertyBagItemTypeEnum key, T defaultValue)
        {
            // Check to see if the item exists in the property bag list
            PropertyBagItem propertyBagItem = commonObject.PropertyBag
                .Where(item => item.ItemType.PropertyType == key)
                .FirstOrDefault();

            // The result to return
            T result;

#warning [Make the string to char a base class extension later if you find it needed]
            // Do a sanity check for strings first incase a string was saved
            // in to the the property bag but a char is requested
            if (typeof(T) == typeof(Char) &&
                propertyBagItem?.Value.GetType() == typeof(String))
            {
                // Is this string not empty?
                if (propertyBagItem.Value.ToString().Length != 0)
                    result = (T)((Object)propertyBagItem.Value.ToString().ToCharArray()[0]);
                else
                    result = (T)propertyBagItem.Value; // Return the origional value
            }
            else
                result = (T)((propertyBagItem == null) ? defaultValue : propertyBagItem.Value);

            return result; // Send the found item back
        }

        /// <summary>
        /// Set the value of the property bag item
        /// </summary>
        /// <typeparam name="T">The type of data being sent</typeparam>
        /// <param name="key">The key of the item to be changed</param>
        /// <param name="value"></param>
        /// <returns>Success or not</returns>
        public Boolean Set<T>(PropertyBagItemTypeEnum key, T value)
        {
            Boolean result = false;

            // Scan the property bag and change all the items that match
            commonObject.PropertyBag
                .ForEach(item =>
                {
                    if (item.ItemType.PropertyType == key)
                    {
                        item.Value = value;
                        result = true; // Set as a success
                    }
                });
            
            // Tell the caller if it worked or not
            return result;
        }
    }
}
