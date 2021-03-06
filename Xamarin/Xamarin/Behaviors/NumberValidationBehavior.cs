﻿using System.Text.RegularExpressions;
using Xamarin.Forms;

namespace Xamarin.Behaviors
{
    public class NumberValidationBehavior : Behavior<Entry>
    {
        protected override void OnAttachedTo(Entry entry)
        {
            entry.TextChanged += OnEntryTextChanged;
            base.OnAttachedTo(entry);
        }

        protected override void OnDetachingFrom(Entry entry)
        {
            entry.TextChanged -= OnEntryTextChanged;
            base.OnDetachingFrom(entry);
        }

        void OnEntryTextChanged(object sender, TextChangedEventArgs args)
        {
            if (args.NewTextValue == null) return;

            var regex = new Regex(@"^\d+(?:\.\d+)?(?:\,\d+)?$");

            var isValid = regex.IsMatch(args.NewTextValue);

            ((Entry) sender).TextColor = isValid ? Color.Green : Color.Red;
        }
    }
}