mergeInto(LibraryManager.library, {
    GetToken: function() {
        var token = window.unityToken || "";
        var bufferSize = lengthBytesUTF8(token) + 1;
        var buffer = _malloc(bufferSize);
        stringToUTF8(token, buffer, bufferSize);
        return buffer;
    }
});