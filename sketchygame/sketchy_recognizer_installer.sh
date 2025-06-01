#!/bin/bash

# Get the repository
git clone https://github.com/maksblink/sketchy_recogniser.git

# Change cwd to project
cd sketchy_recogniser || exit 1

# Install poetry
curl -sSL https://install.python-poetry.org | python3 -

# Create a new conda environment with Python 3.12.8
conda create --yes --name .venv --prefix $(pwd) python=3.12.8

# Get the path to the new Python interpreter
PYTHON_PATH="$(conda run -n .venv which python)"

# Set Poetry to use the new Python interpreter
poetry env use "$PYTHON_PATH"

# Install dependencies
poetry install

# Run the main script
poetry run python main.py