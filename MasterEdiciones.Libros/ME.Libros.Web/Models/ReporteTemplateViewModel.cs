using Rotativa.Options;

namespace ME.Libros.Web.Models
{
    public abstract class ReporteTemplateViewModel
    {
        protected const string WhiteSpace = " ";
        protected const string PrintMediaType = "--print-media-type";
        protected const string FooterRight = "--footer-right";
        protected const string FooterCenter = "--footer-center";
        protected const string FooterLine = "--footer-line";
        protected const string FooterFontSize = "--footer-font-size";
        protected const string FooterFontName = "--footer-font-name";

        protected string FooterRightValue { get; set; }
        protected string FooterCenterValue { get; set; }
        protected string FooterFontSizeValue { get; set; }
        protected string FooterFontNameValue { get; set; }
        protected string Footer { get; set; }

        public Orientation PageOrientation { get; set; }
        public Size PageSize { get; set; }
        public Margins PageMargins { get; set; }
        public string CustomSwitches { get; set; }

        protected ReporteTemplateViewModel()
        {
            PageOrientation = Orientation.Portrait;
            PageSize = Size.A4;
            PageMargins = new Margins(10, 10, 10, 10);
            FooterRightValue = "\"[date] [time]\"";
            FooterFontSizeValue = "\"8\"";
            FooterFontNameValue = "\"calibri light\"";
        }

        public virtual void SetReportConfigurations()
        {
            // Build Footer
            Footer = (!string.IsNullOrEmpty(FooterRightValue)
                         ? WhiteSpace + FooterRight + WhiteSpace + FooterRightValue
                         : "") +
                     WhiteSpace + FooterLine +
                     (!string.IsNullOrEmpty(FooterCenterValue)
                         ? WhiteSpace + FooterCenter + WhiteSpace + FooterCenterValue
                         : "") +
                     WhiteSpace + FooterFontSize + WhiteSpace + FooterFontSizeValue +
                     WhiteSpace + FooterFontName + WhiteSpace + FooterFontNameValue;

            CustomSwitches = PrintMediaType + WhiteSpace + Footer;
        }
    }

    public class ChequeraPdfViewModel : ReporteTemplateViewModel
    {
        public ChequeraPdfViewModel()
        {
            FooterCenterValue = "\"[page] de [toPage]\"";
        }
    }
}