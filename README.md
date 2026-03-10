EcoWatt: High-Voltage Energy Dashboard

Group FINWISE SDG Goal: 13 - Climate Action (Target 13.3)



Project Overview

EcoWatt is a C# application built to track and visualize household energy consumption. The goal is to make wattage usage transparent, helping users develop energy-saving habits that align with the UN Sustainability goals.



N-Tier Architecture

We followed the ITELEC2 standards to structure this project properly:



Presentation (EcoWatt.UI): Contains the WPF interface and user screens.



Business Logic (EcoWatt.BLL): Handles the core security (SHA256) and calculation algorithms.



Data Access (EcoWatt.DAL): Manages the database using EF Core and SQLite.



Models (EcoWatt.Models): Contains the shared entities used across the system.



Repository Structure



CODE/ - Main source files and projects.



INPUT\_DATA/ - SQLite database (inventory.db) and backup JSON files.



DOCUMENTATION/ - SDAD PDF, ERD, and other required diagrams.



Individual Contributions

Ani, Moeses Darwin (System Development - Coding) Implemented the core functionalities of the system. Managed the program structure, operations, business logic, and overall system testing.



Delos Santos, Peter John (Project Support) Handled project coordination and helped prepare various requirements during the development phase.



Gentiroy, Earl Vincent (System Design and Diagrams) In charge of creating the system diagrams like the ERD, as well as the editing and graphical design components.



Rivera, Eunice Jade C. (Documentation and Finalization) Drafted the SDAD documentation and presentation script. Managed the formatting of diagrams and the finalization of all project requirements.

