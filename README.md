MonoTouch.Nimbus
================

iOS Nimbus bindings for the MonoTouch framework.

## Universal Library

Nimbus does not officially support universal libraries (for simulator/device). 
They encourage you to simple include the source into your project which is reasonable.


However, using MonoTouch, we need a compiled library. I created a Git repo that has included the entire Nimbus feature-set into a single compiled objective library.

https://github.com/theonlylawislove/Nimbus-Universal

The compiled version of the repo above is included in this repository and linked to using MonoTouch's LinkWith.

## Single library with all features

Nimbus encourages using modules as needed. To make things simpler, there will be a single MonoTouch.Nimbus library that contains all the modules. To use Nimbus's modular approach with seperate linked assemblies would make things confusing in MonoTouch. I would accept a fork if someone would be willing to, at a later point, create a modular approach. I don't even know if it is possible to, for example, create a "Core" binding that is referenced from other Binding Projects.

## Modules included

There are many modules/classes/protocols and I am only one person. I will be creating bindings on an as needed basis (for my current project). Below is a list of all the modules with an indication as to what module has bindings. 

I encourage you to add bindings to items/modules as you need them! The goal here is to share the Nimbus goodness with the community!

* Core - __PARTIALLY__ (as needed per other modules)
* NetworkImage - __DONE__
* Badge - NOT DONE
* Collections - NOT DONE
* Css - NOT DONE
* InterApp - NOT DONE
* Launcher - NOT DONE
* Models - NOT DONE
* NetworkControllers - NOT DONE
* Overview - NOT DONE
* PagingScrollView - NOT DONE
* Photos - NOT DONE
* TextField - NOT DONE
* WebController - NOT DONE

As you can see, there isn't a lot done at the moment, but the universal library is built and ready for bindings to be added. Simply clone this repo and add your bindings! I will be adding more because my project requires 4-5 of the modules.

__NOTE__: Xamarin released a new tool that makes creating bindings VERY easy. It is called ObjectiveSharpie. Checkout it out!

http://docs.xamarin.com/guides/ios/advanced_topics/binding_objective-c_libraries/objective_sharpie
