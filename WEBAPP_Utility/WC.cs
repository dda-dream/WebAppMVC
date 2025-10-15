using System.Collections.ObjectModel;

namespace WebAppMVC_Utility
{
    public static class WC
    {
        public const string ImagePath = @"\images\product\";
        public const string SessionCart = "ShoppingCartSession";

        public const string SessionOrderId = "ShoppingCartSessionId";


        public const string AdminRole = "Admin";
        public const string CustomerRole = "Customer";


        public const string AdminEmail = "admin@fbdda.duckdns.org";

        public const string Success = "Success";
        public const string Error = "Error";

        public const string StatusPending = "Pending";
        public const string StatusApproved = "Approved";
        public const string StatusInProcess = "Processing";
        public const string StatusShipped = "Shipped";
        public const string StatusCancelled = "Cancelled";
        public const string StatusRefunded = "Refunded";

        public static readonly IEnumerable<string> listStatus = new ReadOnlyCollection<string>(
            new List<string>
            {
                StatusApproved, StatusCancelled, StatusPending, StatusInProcess, StatusShipped, StatusRefunded
            });

    }
}
