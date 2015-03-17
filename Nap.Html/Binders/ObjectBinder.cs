using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using HtmlAgilityPack;

using Nap.Html.Binders.Base;

namespace Nap.Html.Binders
{
	public class ObjectBinder : IBinder
	{
		public object Handle(string input, HtmlNode context, Type outputType)
		{
			var properties = outputType.GetProperties().Where(p => p.CanWrite);
			foreach (var property in properties)
			{
			}
		}
	}
}
