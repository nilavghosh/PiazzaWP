using GalaSoft.MvvmLight.Ioc;
using Sample.View.Components;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Piazza.Extensions
{
    public class Extensions
    {
        private static Extensions instance;
        public static CoreDispatcher _dispatcher { get; set; }

        private Extensions() {
        }

        public static Extensions Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new Extensions();
                }
                return instance;
            }
        }

        public static string GetHTML(DependencyObject obj)
        {
            return (string)obj.GetValue(HTMLProperty);
        }

        public static void SetHTML(DependencyObject obj, string value)
        {
            obj.SetValue(HTMLProperty, value);
        }

        // Using a DependencyProperty as the backing store for HTML.  This enables animation, styling, binding, etc... 
        public static readonly DependencyProperty HTMLProperty =
            DependencyProperty.RegisterAttached("HTML", typeof(string), typeof(Extensions), new PropertyMetadata(0, new PropertyChangedCallback(OnHTMLChanged)));

        private static void OnHTMLChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            //http://www.ben.geek.nz/2010/07/integrated-links-and-styling-for-windows-phone-7-webbrowser-control/
            //WebView wv = d;// (WebView)page.FindName("webView");
            SizeableWebView wv = d as SizeableWebView;
            if (wv != null)
            {
                wv.NavigateToContent(HtmlAgilityPack.HtmlEntity.DeEntitize((string)e.NewValue));
                //wv.NavigationCompleted +=wv_NavigationCompleted;
                //string returnStr = await theWebView.InvokeScriptAsync("eval", new string[] { SetBodyOverFlowHiddenString });
            }
        }

        static void wv_NavigationCompleted(WebView sender, WebViewNavigationCompletedEventArgs args)
        {
            Task.Run(() => InjectScript((WebView)sender));
        }
        static string SetBodyOverFlowHiddenString = @"function SetBodyOverFlowHidden()
        {
            document.body.style.overflow = 'hidden';
            return 'Set Style to hidden';
        } 
        // now call the function!
        SetBodyOverFlowHidden();";


        static string DisableScrollingJs = @"function RemoveScrolling()
                              {
                                  var styleElement = document.createElement('style');
                                  var styleText = 'body, html { overflow: hidden; }'
                                  var headElements = document.getElementsByTagName('head');
                                  styleElement.type = 'text/css';
                                  if (headElements.length == 1)
                                  {
                                      headElements[0].appendChild(styleElement);
                                  }
                                  else if (document.head)
                                  {
                                      document.head.appendChild(styleElement);
                                  }
                                  if (styleElement.styleSheet)
                                  {
                                      styleElement.styleSheet.cssText = styleText;
                                  }
                              }";


        static int count;
        private async static void InjectScript(WebView wv)
        {
            _dispatcher = wv.Dispatcher;
            await _dispatcher.RunAsync(CoreDispatcherPriority.High, async () =>
                        {
                            await wv.InvokeScriptAsync("eval", new string[] { SetBodyOverFlowHiddenString });
                        });
        }
    }
}
