namespace Business.Constants
{
    public static class Messages
    {
        #region Product
        public static string ProductAdded = "Ürün başarılı bir şekilde eklendi.";
        public static string ProductUpdated = "Ürün başarılı bir şekilde güncellendi.";
        public static string ProductDeleted = "Ürün başarılı bir şekilde silindi.";
        public static string ProductNameAlreadyExists = "Aynı ürün isimde kayıt bulunmaktadır.";
        #endregion

        #region Category
        public static string CategoryAdded = "Kategori başarılı bir şekilde eklendi";
        public static string CategoryUpdated = "Kategori başarılı bir şekilde güncellendi";
        public static string CategoryDeleted = "Kategori başarılı bir şekilde silindi";
        public static string CategoryNameAlreadyExists = "Aynı kategori isminde kayıt bulunmaktadır.";
        public static string CategoryIsEnabled = "Kategori aktif değildir.";
        #endregion

        #region Auth
        public static string UserAdded = "Kullanıcı başarılı bir şekilde eklendi.";
        public static string UserNotFound = "Kullanıcı bulunamadı.";
        public static string PasswordError = "Şifre yanlış";
        public static string SuccessfulLogin = "Sisteme giriş başarılı";
        public static string UserExists = "Böyle bir kullanıcı mevcuttur.";
        public static string UserRegistered = "Kullanıcı başarılı bir şekilde kaydedildi.";
        public static string AccessTokenCreated = "Access token başarıyla oluşturuldu.";
        public static string AuthorizationDenied = "Yetkiniz yok.";
        #endregion
    }
}
