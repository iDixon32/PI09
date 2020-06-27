$(function () {
    $(document).on('click', '.delete', function (event) { //svaki put kad se pojavi dokument s određenim stilom ovo će se obaviti
      if (!confirm("Obrisati zapis?")) {
        event.preventDefault();
      }
    });
  });