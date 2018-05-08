(function(){Type.registerNamespace("Telerik.Web.UI");
var a=Telerik.Web.UI;
Telerik.Web.UI.TreeListMobileEditView=function(b){this._editIndex=null;
this._insertIndex=null;
this._ownerId=null;
a.TreeListMobileEditView.initializeBase(this,[b]);
};
a.TreeListMobileEditView.prototype={initialize:function(){a.TreeListMobileEditView.callBaseMethod(this,"initialize");
},dispose:function(){a.TreeListMobileEditView.callBaseMethod(this,"dispose");
},get_owner:function(){if(!this._owner){this._owner=$find(this._ownerId);
}return this._owner;
},onInit:function(){this.changed();
},applyChanges:function(){if(this._editIndex){this.get_owner().fireCommand("Update",this._editIndex);
}else{if(this._insertIndex){this.get_owner().fireCommand("PerformInsert",this._insertIndex);
}else{this.get_owner().fireCommand("PerformInsert","");
}}},show:function(){a.TreeListMobileEditView.callBaseMethod(this,"show");
},close:function(){if(this._editIndex){this.get_owner().fireCommand("Cancel",this._editIndex);
}else{if(this._insertIndex){this.get_owner().fireCommand("CancelInsert",this._insertIndex);
}else{this.get_owner().fireCommand("CancelInsert","");
}}}};
a.TreeListMobileEditView.registerClass("Telerik.Web.UI.TreeListMobileEditView",Telerik.Web.UI.TreeListMobileView);
})();