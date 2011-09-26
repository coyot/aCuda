@echo off


pdflatex thesis-master.tex 
bibtex   thesis-master
pdflatex thesis-master.tex 
pdflatex thesis-master.tex 


del *.aux *.bak *.log *.blg *.bbl *.toc *.out