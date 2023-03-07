function showAlert(title, text, icon, showCancelButton, confirmButtonText) {
  return Swal.fire({
    title,
    text,
    icon,
    showCancelButton,
    confirmButtonText,
  }).then((result) => result.isConfirmed);
}
