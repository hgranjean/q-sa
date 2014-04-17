using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Rules.WebEditor.Models
{
    public class BladeEditorViewModel
    {
        public BladeViewModel Blade { get; set; }
        public String Body { get; set; }

        public BladeEditorViewModel(BladeViewModel bladeViewModel)
        {
            this.Blade = bladeViewModel;

            //this.Body = this.Blade.Name;

            this.Body = @"
function getCompletions(token, context) {
  var found = [], start = token.string; token. 
  function maybeAdd(str) {
    if (str.indexOf(start) == 0) found.push(str);
  }
  function gatherCompletions(obj) {
    if (typeof obj == ""string"") forEach(stringProps, maybeAdd);
    else if (obj instanceof Array) forEach(arrayProps, maybeAdd);
    else if (obj instanceof Function) forEach(funcProps, maybeAdd);
    for (var name in obj) maybeAdd(name);
  }

  if (context) {
    // If this is a property, see if it belongs to some object we can
    // find in the current environment.
    var obj = context.pop(), base;
    if (obj.className == ""js-variable"")
      base = window[obj.string];
    else if (obj.className == ""js-string"")
      base = "";
    else if (obj.className == ""js-atom"")
      base = 1;
    while (base != null && context.length)
      base = base[context.pop().string];
    if (base != null) gatherCompletions(base);
  }
  else {
    // If not, just look in the window object and any local scope
    // (reading into JS mode internals to get at the local variables)
    for (var v = token.state.localVars; v; v = v.next) maybeAdd(v.name);
    gatherCompletions(window);
    forEach(keywords, maybeAdd);
  }
  return found;
}
";
        }
    }
}