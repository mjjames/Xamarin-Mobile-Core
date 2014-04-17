using MonoTouch.UIKit;
using System.Collections.Generic;
using System.Drawing;

namespace MKS.Mobile.Core.iOS.Views
{
    public class NumericEntryElement : EntryWithValidationElement
    {

        public NumericEntryElement(string caption, string placeholder, string value)
            : base(caption, placeholder, value)
        {
            base.KeyboardType = UIKeyboardType.DecimalPad;
        }

        protected override UITextField CreateTextField(RectangleF frame)
        {
            var textField = base.CreateTextField(frame);

            var toggleNumberType = new UIBarButtonItem("+/-", UIBarButtonItemStyle.Bordered, (sender, args) => NegateValue());

            var toolbar = new UIToolbar(new RectangleF(0, 0, frame.Width, 30f))
            {
                Translucent = true
            };


            var items = new List<UIBarButtonItem>();
            items.Add(toggleNumberType);
            items.Add(new UIBarButtonItem(UIBarButtonSystemItem.FlexibleSpace));

            if (ReturnKeyType.HasValue && ReturnKeyType.Value == UIReturnKeyType.Done)
            {

                var done = new UIBarButtonItem(UIBarButtonSystemItem.Done);

                done.Clicked += (o, e) =>
                {
                    if (ReturnKeyType.HasValue && ReturnKeyType.Value == UIReturnKeyType.Done)
                    {
                        ResignFirstResponder(true);
                    }
                    else
                    {
                        Parent.GetContainerTableView().NextResponder.BecomeFirstResponder();
                    }
                };
                items.Add(done);
            }

            toolbar.SetItems(items.ToArray(), false);

            textField.InputAccessoryView = toolbar;
            return textField;
        }

        private void NegateValue()
        {
            float value;
            if (float.TryParse(Value, out value))
            {
                Value = (-value).ToString();
            }
        }

        public new UIKeyboardType KeyboardType
        {
            get
            {
                return UIKeyboardType.DecimalPad;
            }
        }

    }
}