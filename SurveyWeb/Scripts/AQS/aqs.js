var focus = 0,
  blur = 0;
$("p")
  .focusout(function () {
      focus++;
      $("#focus-count").text("focusout fired: " + focus + "x");
  })
  .blur(function () {
      blur++;
      $("#blur-count").text("blur fired: " + blur + "x");
  });