namespace Xbim.COBie.Contracts
{
    public interface ICOBieSerialiser
    {
        void Serialise(COBieWorkbook workbook, ICOBieValidationTemplate validationTemplate);
    }
}
