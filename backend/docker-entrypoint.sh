#!/bin/bash

########################################################################################
#   THIS IS A DOCKER ENTRYPOINT FOR THE PYTHON CONTAINER USED IN THE UTP WEB22 COURSE  #
########################################################################################

SCRIPT_BASE=`dirname $0`

cd ${SCRIPT_BASE}

python -m venv .venv
source .venv/bin/activate

pip install --upgrade pip
pip install -r requirements.txt

echo "Waiting for database..."
python ./check_pg_port.py

export FLASK_APP=app
export FLASK_ENV=development
flask run --host=0.0.0.0
#python app.py --host=0.0.0.0

# Daemonize the container
tail -f /dev/null
