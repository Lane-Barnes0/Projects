from tkinter import *
import tkinter as tk
import os, zipfile


class myApp:
    def __init__(self):
        self.root = Tk()
        self.root.title("Zip File Extractor")
        self.root.geometry("500x400")

        directoryNameLabel = Label(self.root, text="Location of Zip Files")
        directoryNameLabel.grid(column=0, row=0)
        self.directoryName = StringVar()
        directoryNameEntry = Entry(self.root, textvariable=self.directoryName)
        directoryNameEntry.grid(column=1, row= 0)

        saveLocationLabel = Label(self.root, text="Save To")
        saveLocationLabel.grid(column=0, row=1)

        self.saveLocationName = StringVar()
        saveLocationNameEntry = Entry(self.root, textvariable=self.saveLocationName)
        saveLocationNameEntry.grid(column=1, row=1)

        extractButton = Button(self.root, text="Extract", command = self.extractAllZip)
        extractButton.grid(column=3, row=1, sticky=(S,E), padx=10, pady=10)

        self.errorName = StringVar()
        errorLabel = Label(self.root, textvariable=self.errorName)
        errorLabel.grid(column=0, row=2)


    def run(self):
        self.root.mainloop()

    def extractAllZip(self):
        try:
           
            if(self.saveLocationName.get() == ""):
                self.errorName.set("Save Location Cannot Be Empty")
            else:
                currentDirectory = os.curdir
                os.chdir(self.directoryName.get())
                for item in os.listdir(self.directoryName.get()):
                    if item.endswith(".zip"):
                        file_name = os.path.abspath(item)
                        zip_ref = zipfile.ZipFile(file_name) 
                        zip_ref.extractall(self.saveLocationName.get())
                        zip_ref.close()
                os.chdir(currentDirectory)
                self.errorName.set("")
        except:
            self.errorName.set("Zip Location Not Found")
    




app = myApp()

app.run()







