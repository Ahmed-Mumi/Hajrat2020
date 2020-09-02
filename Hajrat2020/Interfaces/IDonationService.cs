using Hajrat2020.ViewModel;

namespace Hajrat2020.Interfaces
{
    public interface IDonationService
    {
        DonationViewModel GetDonations(int? TypeOfHelpId, int? FamilyInNeedId, int? page, bool all);
        DonationViewModel AddDonation();
        DonationViewModel EditDonation(int id);
        void DeleteDonation(int id);
        void SaveDonation(DonationViewModel donationViewModel);
        DonationViewModel GetDonation(int id);
    }
}
