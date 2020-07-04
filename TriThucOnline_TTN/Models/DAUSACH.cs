namespace TriThucOnline_TTN.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("DAUSACH")]
    public partial class DAUSACH
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public DAUSACH()
        {
            CT_DONHANG = new HashSet<CT_DONHANG>();
        }

        [Key]
        public int MaSach { get; set; }

        [StringLength(50)]
        public string TenSach { get; set; }

        public int? MaNXB { get; set; }

        public int? MaKhoaHoc { get; set; }

        public int? MaTG { get; set; }

        public int? NamXuatBan { get; set; }

        public double? GiaTien { get; set; }

        public string PicBook { get; set; }

        public int? SoTrang { get; set; }

        public string BoCucSach { get; set; }

        public string TrichDan { get; set; }

        public int? SoLuongTon { get; set; }

        public int? Moi { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CT_DONHANG> CT_DONHANG { get; set; }

        public virtual TACGIA TACGIA { get; set; }

        public virtual KHOAHOC KHOAHOC { get; set; }

        public virtual NXB NXB { get; set; }
    }
}
