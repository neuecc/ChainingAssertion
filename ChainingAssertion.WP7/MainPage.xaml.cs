using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using Microsoft.Silverlight.Testing;

namespace ChainingAssertion.WP7
{
    public partial class MainPage : PhoneApplicationPage
    {
        // コンストラクター
        public MainPage()
        {
            InitializeComponent();

            var testPage = UnitTestSystem.CreateTestPage() as IMobileTestPage;
            this.BackKeyPress += (x, xe) => xe.Cancel = testPage.NavigateBack();
            this.Content = testPage as UIElement;
        }
    }
}