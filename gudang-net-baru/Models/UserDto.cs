using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

public class UserDto
{
    [Required(ErrorMessage = "Nama Depan wajib diisi!")]
    [MaxLength(100, ErrorMessage = "Nama Depan maksimal 100 karakter!")]
    public string FirstName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Nama Belakang wajib diisi!")]
    [MaxLength(100, ErrorMessage = "Nama Belakang maksimal 100 karakter!")]
    public string LastName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Email wajib diisi!")]
    [EmailAddress(ErrorMessage = "Email tidak valid!")]
    public string Email { get; set; } = string.Empty;

    public string? UserName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Password wajib diisi!")]
    [DataType(DataType.Password)]
    [MinLength(6, ErrorMessage = "Password minimal 6 karakter!")]
    public string Password { get; set; } = string.Empty;

    [Required(ErrorMessage = "Konfirmasi Password wajib diisi!")]
    [DataType(DataType.Password)]
    [Compare("Password", ErrorMessage = "Konfirmasi Password dan Password tidak sesuai!")]
    public string ConfirmPassword { get; set; } = string.Empty;

    [Required(ErrorMessage = "Role wajib diisi!")]
    public List<string> SelectedRoleIds { get; set; } = new();

    // properti tambahan opsional
    public bool Status { get; set; }
    public int CreatedBy { get; set; }
    public DateTime CreatedAt { get; set; }
    public int UpdatedBy { get; set; }
    public DateTime UpdatedAt { get; set; }
    public int DeletedBy { get; set; }
    public DateTime DeletedAt { get; set; }

    // ini kelihatannya tidak dipakai
    public List<string> Selected { get; set; } = new();
}
