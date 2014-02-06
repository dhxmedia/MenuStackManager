
CSHARP_FILES = $(wildcard ../Assets/MenuStackManager/Source/*.cs)
CSHARP_DLL = bin/MenuStackManager.dll
CSHARP_CC = gmcs

$(CSHARP_DLL):$(CSHARP_FILES)
	$(CSHARP_CC) -target:library -r:"C:\\Program Files (x86)\\Unity\\Editor\\Data\\Managed\\UnityEngine.dll" -out:$(CSHARP_DLL) $(CSHARP_FILES)
	
all:$(OUTPUT_DYNAMIC) $(CSHARP_DLL)
	@echo "Making bin"
	
clean:
	-rm $(CSHARP_DLL)