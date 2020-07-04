namespace TriThucOnline_TTN.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using System.Web.Mvc;

    [Table("KHOAHOC")]
    public partial class KHOAHOC
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public KHOAHOC()
        {
            DAUSACHes = new HashSet<DAUSACH>();
        }

        [Key]
        public int MaKhoaHoc { get; set; }

        [StringLength(50)]
        [Remote("ExsitsName", "QuanLyKhoaHoc", ErrorMessage = "Tên đã tồn tại", AdditionalFields = "MaKhoaHoc")]
        [Required(ErrorMessage = "Tên không được để trống")]
        public string TenKhoaHoc { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DAUSACH> DAUSACHes { get; set; }
    }
}
