#load "A/LibraryA.fs";;
#load "B/Squarer.fs";;
#load "B/LibraryB.fs";;

B.Worker.doWork 5
    |> printf "%s";;