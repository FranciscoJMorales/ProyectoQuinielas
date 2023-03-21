function showAlert(title, text, icon, showCancelButton, cancelButtonText, confirmButtonText) {
  return Swal.fire({
    title: title,
    text: text,
    icon: icon,
    showCancelButton: showCancelButton,
    cancelButtonText: cancelButtonText,
    confirmButtonText: confirmButtonText,
  }).then((result) => result.isConfirmed);
}
