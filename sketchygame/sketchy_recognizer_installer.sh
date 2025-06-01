#!/bin/bash

# Get the repository
if ! find . -type d -name sketchy_recogniser | grep -q sketchy_recogniser; then
    git clone https://github.com/maksblink/sketchy_recogniser.git
fi

# Change cwd to project
cd sketchy_recogniser || exit 1


if ! command -v poetry &> /dev/null; then
# Install poetry
curl -sSL https://install.python-poetry.org | python3 -
fi

# Create a new conda environment with Python 3.12.8
if ! find . -type d -name .venv | grep -q .venv; then
    conda create --yes --prefix $(pwd)"/.venv" python=3.12.8
fi

# Get the path to the new Python interpreter
PYTHON_PATH="$(find ./.venv -type f -name python3.12)"

# Set Poetry to use the new Python interpreter
poetry env use "$PYTHON_PATH"

# Get to the cwd 
cd ..

# Install dependencies
poetry --directory ./sketchy_recogniser install --no-root

# Run the main script
poetry --directory ./sketchy_recogniser run python main.py