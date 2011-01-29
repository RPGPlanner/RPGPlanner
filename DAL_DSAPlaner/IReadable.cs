using System;
using System.Collections.Generic;
using System.Text;

public interface IReadable
{
    bool isReader(Person forum_user);
    void markAsRead(Person forum_user);

    bool isNew(Person forum_user);
}
