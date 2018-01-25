﻿// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Globalization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Core;

namespace Microsoft.AspNetCore.Mvc.ModelBinding.Internal
{
    internal class HeaderValueProvider : IValueProvider
    {
        private readonly CultureInfo _culture;
        private readonly string _headerFieldName;
        private readonly IHeaderDictionary _headers;

        public HeaderValueProvider(
            IHeaderDictionary headers,
            CultureInfo culture,
            string headerFieldName)
        {
            if (headers == null)
            {
                throw new ArgumentNullException(nameof(headers));
            }

            if (string.IsNullOrEmpty(headerFieldName))
            {
                throw new ArgumentException(Resources.ArgumentCannotBeNullOrEmpty, nameof(headerFieldName));
            }

            _headers = headers;
            _culture = culture;
            _headerFieldName = headerFieldName;
        }

        public bool UseCommaSeparatedValues { get; set; }

        /// <inheritdoc />
        public bool ContainsPrefix(string prefix)
        {
            return _headers.ContainsKey(_headerFieldName);
        }

        /// <inheritdoc />
        public ValueProviderResult GetValue(string key)
        {
            string[] values;
            if (UseCommaSeparatedValues)
            {
                values = _headers.GetCommaSeparatedValues(_headerFieldName);
            }
            else
            {
                values = new[] { (string)_headers[_headerFieldName] };
            }

            if (values.Length == 0)
            {
                return ValueProviderResult.None;
            }
            else
            {
                return new ValueProviderResult(values, _culture);
            }
        }
    }
}