using System;
using System.Collections.Generic;
using System.Text;

namespace ImmersivePresentation
{
    public class PDFElement:Image2D
    {
        public int pdfId;
        public int pdfStart;
        public int stageStart;
        public int pdfCount;
        public int dpi;

        public PDFElement(int id, string pRelativeImageSource, double pXPosition, double pYPosition, double pXScale, double pYScale, int pdfStart, int stageStart, int pdfCount, int dpi) :base(pRelativeImageSource, pXPosition, pYPosition, pXScale, pYScale)
        {
            this.pdfId = id;
            this.pdfStart = pdfStart;
            this.stageStart = stageStart;
            this.pdfCount = pdfCount;
            this.dpi = dpi;
        }
    }
}
