# IoT.Device

IoT.Device is a modular .NET library for unified control and integration of various IoT devices and protocols. It provides abstractions and implementations for device discovery, feature management, and protocol-specific communication, enabling seamless interaction with smart devices from different vendors and ecosystems.

## Features
- **Device Abstraction:** Common interfaces and base classes for IoT devices and their features.
- **Protocol Support:** Extensible support for multiple protocols, including UPnP, Lumi, Yeelight, and more.
- **Device Factories:** Easily instantiate and manage devices using factory patterns.
- **Feature Export:** Attribute-based system for exporting device features and services.
- **Code Generation:** Source generators for device features and protocol models.
- **Extensibility:** Add new device types and protocols with minimal effort.

## Project Structure
- `IoT.Device/` — Core device abstractions, features, and attributes.
- `IoT.Device.Generators/` — Source generators for device features and models.
- `IoT.Device.Lumi/` — Lumi (Xiaomi Gateway, model DGNWG02LM) protocol device implementations.
- `IoT.Device.Upnp/` — UPnP protocol device implementations.
- `IoT.Device.Upnp.Umi/` — Umi (Xiaomi Mi Internet Speaker) protocol device implementations.
- `IoT.Device.Yeelight/` — Yeelight device implementations and features.
- `IoT.Protocol/` — Protocol definitions and related projects.
- `OOs.Common/` — Common utilities and shared code.
- `Samples/` — Example usage and control applications.

## License
This project is licensed under the MIT License.
