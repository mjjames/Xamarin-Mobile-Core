using MonoTouch.Dialog;
using MonoTouch.UIKit;
using System;
using System.Drawing;

namespace MKS.Mobile.Core.iOS.Views
{
    public class EntryWithValidationElement : EntryElement
    {
        public EntryWithValidationElement(string caption, string placeholder, string value)
            : base(caption, placeholder, value)
        {
        }

        public Func<UITextField, bool> Validate { get; set; }

        protected override UITextField CreateTextField(RectangleF frame)
        {
            var textField = base.CreateTextField(frame);
            if (Validate != null)
            {
                SetupValidation(textField);
            }

            return textField;
        }

        private void SetupValidation(UITextField textField)
        {
            var accessoryView = new UILabel
            {
                Text = "\u2B1C "
            };
            accessoryView.SizeToFit();
            textField.RightViewMode = UITextFieldViewMode.Always;
            textField.RightView = accessoryView;
            textField.Ended += (o, e) =>
            {
                if (Validate == null)
                {
                    return;
                }
                accessoryView.Text = Validate(textField) ? "\u2705  " : "\u2B1C  ";
            };
        }

    }
}