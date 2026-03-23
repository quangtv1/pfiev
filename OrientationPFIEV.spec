# -*- mode: python ; coding: utf-8 -*-
from PyInstaller.utils.hooks import collect_data_files, collect_submodules

block_cipher = None

a = Analysis(
    ['main.py'],
    pathex=[],
    binaries=[],
    datas=[
        ('resources', 'resources'),
        ('data', 'data'),
    ] + collect_data_files('matplotlib') + collect_data_files('PySide6'),
    hiddenimports=[
        'PySide6.QtCore',
        'PySide6.QtWidgets',
        'PySide6.QtGui',
        'matplotlib.backends.backend_qtagg',
        'openpyxl',
        'openpyxl.styles',
        'openpyxl.utils',
        'pyodbc',
    ] + collect_submodules('PySide6'),
    hookspath=[],
    hooksconfig={},
    runtime_hooks=[],
    excludes=['tkinter', 'PyQt5', 'PyQt6'],
    win_no_prefer_redirects=False,
    win_private_assemblies=False,
    cipher=block_cipher,
)

pyz = PYZ(a.pure, a.zipped_data, cipher=block_cipher)

exe = EXE(
    pyz,
    a.scripts,
    [],
    exclude_binaries=True,
    name='OrientationPFIEV',
    debug=False,
    bootloader_ignore_signals=False,
    strip=False,
    upx=False,  # UPX disabled to avoid antivirus false positive
    console=False,  # --windowed
    disable_windowed_traceback=False,
    target_arch=None,
    codesign_identity=None,
    entitlements_file=None,
    icon='resources/iconPFEIV.ico',
)

coll = COLLECT(
    exe,
    a.binaries,
    a.zipfiles,
    a.datas,
    strip=False,
    upx=False,
    upx_exclude=[],
    name='OrientationPFIEV',
)
