String.empty = '';

Boolean.parse = function (val) {
    return (/^true$/i).test(val);
}

String.format = function () {
    var s = arguments[0];
    for (var i = 0; i < arguments.length - 1; i++) {
        var reg = new RegExp("\\{" + i + "\\}", "gm");
        s = s.replace(reg, arguments[i + 1]);
    }
    return s;
};

String.prototype.escape = function () {
    return this.replace(/(:|\.|\[|\]|,)/g, "\\$1");
};

Array.prototype.move = function (old_index, new_index) {
    if (new_index >= this.length) {
        var k = new_index - this.length;
        while ((k--) + 1) {
            this.push(undefined);
        }
    }
    this.splice(new_index, 0, this.splice(old_index, 1)[0]);
    return this;
};

Array.prototype.insert = function (index, item) {
    this.splice(index, 0, item);
};

Array.prototype.remove = function (index) {
    this.splice(index, 1);
};


$.fn.bindFirst = function (name, fn) {
    this.bind(name, fn);
    var handlers = $._data(this.get(0), 'events')[name];
    var handler = handlers.pop();
    handlers.insert(0, handler);
};

$.fn.oneFirst = function (name, fn) {
    this.one(name, fn);
    var handlers = $._data(this.get(0), 'events')[name];
    var handler = handlers.pop();
    handlers.insert(0, handler);
};

function serialize(obj) {
    return JSON.stringify(obj, function (key, value) {
        if (typeof value === 'function') {
            return value.toString();
        }
        return value;
    });
}

function deserialize(str) {
    return JSON.parse(str, function (key, value) {
        if (value && typeof value === "string" && value.substr(0, 8) == "function") {
            var startBody = value.indexOf('{') + 1;
            var endBody = value.lastIndexOf('}');
            var startArgs = value.indexOf('(') + 1;
            var endArgs = value.indexOf(')');

            return new Function(value.substring(startArgs, endArgs),
                                value.substring(startBody, endBody));
        }
        return value;
    });
}
