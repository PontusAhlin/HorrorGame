# Adaptive Performance
The Adaptive Performance package provides an API to get feedback about the thermal and power state of mobile devices, enabling applications to make performance-relevant adaptions at runtime.

# Installation
The Adaptive Performance package requires a provider packages such as the *Adaptive Performance Samsung (Android)* package to be installed via the provider section in the Adaptive Performance settings (or via Package Manager).

Alternatively it can also be added manually to the manifest.json of the test project:

{
  "dependencies": {
    "com.unity.adaptiveperformance": "file:../../com.unity.adaptiveperformance",
    "com.unity.adaptiveperformance.samsung.android": "file:../../com.unity.adaptiveperformance.samsung.android",
  }
}

In this case the test project is on the same folder as the checked out Adaptive Performance repositories.

## Internal Candidates registry

This requires VPN access.
If packages are not released yet but promoted to the internal registry for testing it is required to change manifest.json of the test project to search the internal candidates registry https://artifactory.prd.cds.internal.unity3d.com/artifactory/api/npm/upm-candidates :

{
  "registry": "https://artifactory.prd.cds.internal.unity3d.com/artifactory/api/npm/upm-candidates",
  "dependencies": {
    "com.unity.adaptiveperformance": "file:../../com.unity.adaptiveperformance",
    "com.unity.adaptiveperformance.samsung.android": "file:../../com.unity.adaptiveperformance.samsung.android",
    "com.unity.device-simulator": "2.2.4-preview",
  }
}

## Tests

Packages comes with Test Runner integration tests. They can be activated by adding the test package to the manifest.json of the test project.

{
  "dependencies": {
    "com.unity.adaptiveperformance": "file:../../com.unity.adaptiveperformance",
    "com.unity.adaptiveperformance.samsung.android": "file:../../com.unity.adaptiveperformance.samsung.android",
    "com.unity.device-simulator": "2.2.4-preview",
  },
  "testables": [
    "com.unity.adaptiveperformance",
    "com.unity.adaptiveperformance.samsung.android"
  ]
}
