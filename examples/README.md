# Examples

In this directory you can find the following examples:

* **Simple**: shows how to write samples and how to read and take.
* **Mixed**: shows how to write and read samples from complex types
    like sequences and inner structures.
* **Objects**: shows how to write and read samples mapped from C#
    *classes* and *structs*.

Before running the example, make sure to set the native libraries
(_rticonnextdds-connector/lib/arch_) in your load library environment variable:

* Windows: `PATH`
* Linux: `LD_LIBRARY_PATH`
* Mac OS X: `DYLD_LIBRARY_PATH`

For Android projects make sure the library is copied to the APK. The actual
location may change depending on the type of Android project.
