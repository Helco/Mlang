# Mlang

A GLSL preprocessor to ease developing of shaders that:
  - define many variants
  - communicate not only the GPU programs but also the pipeline state
  - should be cached
  - should be reflected

The current language and state however is tailored towards the needs of [my current project](https://github.com/Helco/zzio) so your mileage with this library may vary. In particular:
  - the range of pipeline state represents the features of [Veldrid]([https://github.com](https://github.com/veldrid/veldrid)https://github.com/veldrid/veldrid)
  - the supported stages are vertex and fragment (currently no compute support as well)
