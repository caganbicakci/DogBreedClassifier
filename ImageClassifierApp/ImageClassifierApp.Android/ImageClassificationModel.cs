using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ImageClassifierApp.Droid
{
    public class ImageClassificationModel
    {
        public ImageClassificationModel(string tagName, float probability)
        {
            TagName = tagName;
            Probability = probability;
        }

        public float Probability { get; }
        public string TagName { get; }

    }
}