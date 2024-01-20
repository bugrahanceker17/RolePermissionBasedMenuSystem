using RoleBasedMenuSystem.Core.Entities;

namespace RoleBasedMenuSystem;

public class PermissionProperties : EnumBase
{
    public PermissionProperties(string name, string code) : base(name, code)
    {
    }
   
    public static readonly PermissionProperties SatinAlmaMenusuGoruntuleme = new("Satın Alma Menüsü Görme Yetkisi", PermissionsCode.SatinAlmaMenusuGoruntuleme);
    public static readonly PermissionProperties İstatistikleriGormeMenu = new("İstatistikleri Görme Menü Yetkisi", PermissionsCode.İstatistikleriGormeMenu);
    public static readonly PermissionProperties ProjeMenusuGoruntuleme = new("Proje Menüsünü Görme Yetkisi", PermissionsCode.ProjeMenusuGoruntuleme);
    public static readonly PermissionProperties TumMenuleriGormeAdmin = new("Tüm Menüleri Görme Yetkisi (Admin)", PermissionsCode.TumMenuleriGormeAdmin);
    public static readonly PermissionProperties UrunEklemeYetkisi = new("Ürün Ekleme Yetkisi", PermissionsCode.UrunEklemeYetkisi);
}