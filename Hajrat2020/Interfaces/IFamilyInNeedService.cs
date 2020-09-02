using Hajrat2020.ViewModel;

namespace Hajrat2020.Interfaces
{
    public interface IFamilyInNeedService
    {
        FamilyViewModel GetFamilies(int? page);
        FamilyViewModel AddFamily();
        void DeActivateFamily(int id);
        FamilyViewModel EditFamily(int id);
        void SaveFamily(FamilyViewModel familyViewModel);
        FamilyViewModel GetFamily(int id);
        void AddFamilyToPrint(int id);
        void RemoveAllFamilyToPrint();
        void RemoveFamilyToPrint(int id);
    }
}
