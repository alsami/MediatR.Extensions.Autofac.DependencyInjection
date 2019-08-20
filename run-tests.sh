#!/usr/bin/env bash
for d in test/*; 
    do 
        if [[ ${d} == *.Tests ]]; then
            cd ${d} 
            dotnet test
            if [[ ${?} != 0  ]]
            then
                exit -1
            fi
            cd ../../;
        fi 
done
