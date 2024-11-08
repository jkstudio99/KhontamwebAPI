# KhontamWebAPI

`KhontamWebAPI` is a backend API for managing product data, designed to interact with the `KhontamwebAngularTest-V.1.0.0` front-end application. This API provides CRUD operations for managing products, with data stored in a MySQL database and media files handled by Cloudinary.

## Table of Contents

- [Project Overview](#project-overview)
- [Features](#features)
- [Prerequisites](#prerequisites)
- [Installation](#installation)
- [Configuration](#configuration)
- [Usage](#usage)
- [API Endpoints](#api-endpoints)
- [Technologies](#technologies)
- [License](#license)

## Project Overview

`KhontamWebAPI` serves as the data layer for the Khontamweb product management application, offering RESTful API endpoints for product operations like adding, viewing, updating, and deleting products.

## Features

- **Add Product**: Adds new products with details including name, category, description, status, and image.
- **View Products**: Retrieves all products or a specific product by ID.
- **Update Product**: Updates product details.
- **Delete Product**: Deletes a product from the system.
- **Image Management**: Supports image upload and storage with Cloudinary integration.

## Prerequisites

- **.NET SDK**: v8.0 or later
- **MySQL**: For storing product data
- **Cloudinary Account**: For media management
- **Environment Variables**: Set up for database and Cloudinary credentials

## Installation

1. Clone the repository:
   ```bash
   git clone https://github.com/yourusername/KhontamWebAPI.git
   cd KhontamWebAPI
