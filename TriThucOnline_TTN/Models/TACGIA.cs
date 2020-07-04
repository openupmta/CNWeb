namespace TriThucOnline_TTN.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using System.Web.Mvc;

    [Table("TACGIA")]
    public partial class TACGIA
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public TACGIA()
        {
            DAUSACHes = new HashSet<DAUSACH>();
        }

        [Key]
        public int MaTG { get; set; }

        [StringLength(50)]
        [Remote("ExsitsName", "QuanLyTacGia", ErrorMessage = "Tác giả đã tồn tại", AdditionalFields = "MaTG")]
        [Required(ErrorMessage = "Email không được để trống")]
        public string TenTG { get; set; }

        public string ThongTinGioiThieu { get; set; }

        public string PicTG { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DAUSACH> DAUSACHes { get; set; }
    }
}
