using System.Web.Optimization;

namespace ME.Libros.Web
{
	public class TypeaheadBundleConfig
	{
		public static void RegisterBundles()
		{
			BundleTable.Bundles.Add(new ScriptBundle("~/bundles/typeahead").Include("~/Scripts/typeahead.bundle*"));
			BundleTable.Bundles.Add(new ScriptBundle("~/bundles/typeahead-bloodhound").Include("~/Scripts/bloodhound*"));
			BundleTable.Bundles.Add(new ScriptBundle("~/bundles/typeahead-jquery").Include("~/Scripts/typeahead.jquery*"));
		}
	}
}
