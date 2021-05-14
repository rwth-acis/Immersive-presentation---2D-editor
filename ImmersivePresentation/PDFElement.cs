using System;
using System.Collections.Generic;
using System.Text;

namespace ImmersivePresentation
{
    class PDFElement:Image2D
    {

        public bool useIndividualMapping;
        public int firstPDFPage;
        public int lastPDFPage;
        public int firstStageNumber;
        public bool useIndividualDPI;
        public int dpi;

        public PDFElement(double pXPosition, double pYPosition, double pXScale, double pYScale, bool useIndividualMapping, int firstPDFPage, int lastPDFPage, int firstStageNumber, bool useIndividualDPI, int dpi) :base(pXPosition, pYPosition, pXScale, pYScale)
        {
            this.useIndividualMapping = useIndividualMapping;
            this.firstPDFPage = firstPDFPage;
            this.lastPDFPage = lastPDFPage;
            this.firstStageNumber = firstStageNumber;
            this.useIndividualDPI = useIndividualDPI;
            this.dpi = dpi;
        }
    }
}
