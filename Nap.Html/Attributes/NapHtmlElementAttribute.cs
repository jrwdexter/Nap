using Nap.Html.Attributes.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nap.Html.Attributes
{
	public class HtmlElementAttribute : BaseHtmlAttribute
	{
		private readonly string _selector;

		public HtmlElementAttribute(string selector)
		{
			_selector = selector;
		}

		public string Selector  => _selector;
	}
}
