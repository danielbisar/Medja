# PrintMenu format

The format is simple:

```
STRING, must be enclosed in "...". f.e. "My title"
[optional item]
{ } actual brackets, define items
```

Item with no children

```
{ MENU_TITLE [KeyboardShortcut, [DESCRIPTION]] }
```

Item with with children

```
{ MENU_TITLE [KeyboardShortcut, [DESCRIPTION]]
    { CHILD_TITLE [KeyboardShortcut, [DESCRIPTION]]
        { ANOTHER_CHILD ... }
    } 
}
```
